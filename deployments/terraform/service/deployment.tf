resource "kubernetes_deployment" "this" {
  metadata {
    name      = "${local.name}-deployment"
    namespace = var.namespace
  }

  spec {
    replicas = 1

    selector {
      match_labels = {
        "app" = local.name
      }
    }

    template {
      metadata {
        labels = {
          "app" = local.name
        }
      }

      spec {
        container {
          name  = local.name
          image = var.image


          dynamic "env" {
            for_each = var.value_env
            content {
              name  = env.value.name
              value = env.value.value
            }
          }

          dynamic "env" {
            for_each = var.secret_key_ref_env
            content {
              name = env.value.env_name
              value_from {
                secret_key_ref {
                  name = env.value.secret_name
                  key  = env.value.secret_key
                }
              }
            }
          }

          dynamic "env" {
            for_each = var.config_map_key_ref_env
            content {
              name = env.value.env_name
              value_from {
                config_map_key_ref {
                  name = env.value.config_map_name
                  key  = env.value.config_map_key
                }
              }
            }
          }
        }
      }
    }
  }
}
