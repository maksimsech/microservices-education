locals {
  db_mount_name = "${var.name}mount"
}

resource "kubernetes_stateful_set" "this" {
  metadata {
    name      = "${var.name}-ss"
    namespace = var.namespace
  }

  spec {
    service_name = local.service_name

    selector {
      match_labels = {
        "app" = var.name
      }
    }

    template {
      metadata {
        labels = {
          "app" = var.name
        }
      }

      spec {
        container {
          name  = "mssql"
          image = "mcr.microsoft.com/azure-sql-edge"

          port {
            container_port = 1433
          }

          env {
            name  = "MSSQL_PID"
            value = "Developer"
          }

          env {
            name  = "ACCEPT_EULA"
            value = "Y"
          }

          env {
            name = "MSSQL_SA_PASSWORD"
            value_from {
              secret_key_ref {
                name = kubernetes_secret.this.metadata[0].name
                key  = local.secret_password_key
              }
            }
          }

          volume_mount {
            name       = local.db_mount_name
            mount_path = "/var/opt/mssql/data"
          }
        }

        volume {
          name = local.db_mount_name

          persistent_volume_claim {
            claim_name = kubernetes_persistent_volume_claim.this.metadata[0].name
          }
        }
      }
    }
  }
}
