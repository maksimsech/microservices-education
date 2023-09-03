resource "kubernetes_config_map" "command_configmap" {
  metadata {
    name      = "command-configmap"
    namespace = local.default_namespace
  }

  data = {
    "http-port" = "80"
    "grpc-port" = "81"
    "base-url"  = "http://command-clusterip-service"
  }
}

resource "kubernetes_deployment" "command_deployment" {
  metadata {
    name      = "command-deployment"
    namespace = local.default_namespace
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        "app" = "command-service"
      }
    }

    template {
      metadata {
        labels = {
          "app" = "command-service"
        }
      }

      spec {
        container {
          name  = "command-service"
          image = "maksimsech/command-service:latest-arm"

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
            name = "Grpc__Platform"
            value_from {
              config_map_key_ref {
                name = "platform-configmap"
                key  = "grpc-url"
              }
            }
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "command_clusterip" {
  metadata {
    name      = "command-clusterip-service"
    namespace = local.default_namespace
  }

  spec {
    type = "ClusterIP"

    selector = {
      "app" = "command-service"
    }

    port {
      name        = "http"
      protocol    = "TCP"
      port        = 80
      target_port = 80
    }
  }
}
