resource "kubernetes_namespace" "this" {
  metadata {
    name = var.namespace_name

    labels = {
      name = var.namespace_name
    }
  }
}

module "rabbitmq" {
  source = "./rabbitmq"

  namespace = local.default_namespace
  name      = "rabbitmq"

  management_port = 15672
  messaging_port  = 5672

  share_via_nodeport = true
}

module "mssql" {
  source = "./mssql"

  namespace = local.default_namespace
  name      = "mssql"

  port     = 1433
  password = base64encode(var.mssql_sa_password)
}

module "command_service" {
  source = "./service"

  namespace = local.default_namespace
  name      = "command"

  image = "maksimsech/command-service:latest-arm"

  clusterip_ports = [
    {
      name        = "http"
      protocol    = "TCP"
      port        = 80
      target_port = 80
    }
  ]

  config_data = {
    "http-port" = "80"
    "grpc-port" = "81"
    "base-url"  = "http://${module.command_service.service_name}"
  }

  config_map_key_ref_env = [
    {
      env_name        = "RabbitMq__Host"
      config_map_name = module.rabbitmq.configmap_name
      config_map_key  = module.rabbitmq.config_host_key
    },
    {
      env_name        = "RabbitMq__Port"
      config_map_name = module.rabbitmq.configmap_name
      config_map_key  = module.rabbitmq.config_port_key
    },
    {
      env_name        = "Grpc__Platform"
      config_map_name = module.platform_service.configmap_name
      config_map_key  = "grpc-url"
    }
  ]
}

module "platform_service" {
  source = "./service"

  namespace = local.default_namespace
  name      = "platform"

  image = "maksimsech/platform-service:latest-arm"

  clusterip_ports = [
    {
      name        = "http"
      protocol    = "TCP"
      port        = 80
      target_port = 80
    },
    {
      name        = "grpc"
      protocol    = "TCP"
      port        = 81
      target_port = 81
    }
  ]

  config_data = {
    "http-port" = "80"
    "grpc-port" = "81"
    "grpc-url"  = "http://${module.platform_service.service_name}:81"
  }

  secret_key_ref_env = [
    {
      env_name    = "SqlConnectionString__Password"
      secret_name = module.mssql.secret_name
      secret_key  = module.mssql.secret_password_key
    }
  ]

  value_env = [
    {
      name  = "SqlConnectionString__UserId"
      value = "sa"
    }
  ]

  config_map_key_ref_env = [
    {
      env_name        = "HttpCommandSyncService__BaseAddress"
      config_map_name = module.command_service.configmap_name
      config_map_key  = "base-url"
    },
    {
      env_name        = "RabbitMq__Host"
      config_map_name = module.rabbitmq.configmap_name
      config_map_key  = module.rabbitmq.config_host_key
    },
    {
      env_name        = "RabbitMq__Port"
      config_map_name = module.rabbitmq.configmap_name
      config_map_key  = module.rabbitmq.config_port_key
    },
    {
      env_name        = "Ports__Http"
      config_map_name = module.platform_service.configmap_name
      config_map_key  = "http-port"
    },
    {
      env_name        = "Ports__Grpc"
      config_map_name = module.platform_service.configmap_name
      config_map_key  = "grpc-port"
    },
    {
      env_name        = "SqlConnectionString__Server"
      config_map_name = module.mssql.configmap_name
      config_map_key  = module.mssql.config_db_url_key
    }
  ]
}
