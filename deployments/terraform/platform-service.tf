resource "kubernetes_config_map" "platform_configmap" {
  metadata {
    name      = "platform-configmap"
    namespace = local.default_namespace
  }

  data = {
    "http-port" = "80"
    "grpc-port" = "81"
    "grpc-url"  = "http://platform-clusterip-service:81"
  }
}

resource "kubernetes_deployment" "platform_deployment" {
  metadata {
    name      = "platform-deployment"
    namespace = local.default_namespace
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        "app" = "platform-service"
      }
    }

    template {
      metadata {
        labels = {
          "app" = "platform-service"
        }
      }

      spec {
        container {
          name  = "platform-service"
          image = "maksimsech/platform-service:latest-arm"

          env {
            name = "HttpCommandSyncService__BaseAddress"
            value_from {
              config_map_key_ref {
                name = "command-configmap"
                key  = "base-url"
              }
            }
          }

          env {
            name = "SqlConnectionString__Server"
            value_from {
              config_map_key_ref {
                name = "mssql-configmap"
                key  = "database-url"
              }
            }
          }

          env {
            name  = "SqlConnectionString__UserId"
            value = "sa"
          }

          env {
            name = "SqlConnectionString__Password"
            value_from {
              secret_key_ref {
                name = "mssql-secret"
                key  = "sa-password"
              }
            }
          }

          env {
            name = "RabbitMq__Host"
            value_from {
              config_map_key_ref {
                name = "rabbitmq-configmap"
                key  = "host"
              }
            }
          }

          env {
            name = "RabbitMq__Port"
            value_from {
              config_map_key_ref {
                name = "rabbitmq-configmap"
                key  = "port"
              }
            }
          }

          env {
            name = "Ports__Http"
            value_from {
              config_map_key_ref {
                name = "platform-configmap"
                key  = "http-port"
              }
            }
          }

          env {
            name = "Ports__Grpc"
            value_from {
              config_map_key_ref {
                name = "platform-configmap"
                key  = "grpc-port"
              }
            }
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "platform_clusterip" {
  metadata {
    name      = "platform-clusterip-service"
    namespace = local.default_namespace
  }

  spec {
    type = "ClusterIP"

    selector = {
      "app" = "platform-service"
    }

    port {
      name        = "http"
      protocol    = "TCP"
      port        = 80
      target_port = 80
    }

    port {
      name        = "grpc"
      protocol    = "TCP"
      port        = 81
      target_port = 81
    }
  }
}
