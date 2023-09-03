resource "kubernetes_config_map" "mssql" {
  metadata {
    name      = "mssql-configmap"
    namespace = local.default_namespace
  }

  data = {
    "database-url" = "mssql-clusterip-service, 1433"
  }
}

resource "kubernetes_secret" "mssql" {
  metadata {
    name      = "mssql-secret"
    namespace = local.default_namespace
  }

  type = "Opaque"
  data = {
    "sa-password" = base64encode(var.mssql_sa_password)
  }
}

resource "kubernetes_persistent_volume_claim" "msql" {
  metadata {
    name      = "mssql-pvc"
    namespace = local.default_namespace
  }

  spec {
    resources {
      requests = {
        "storage" = "200Mi"
      }
    }
    access_modes = ["ReadWriteMany"]
  }
}

resource "kubernetes_service" "mssql_nodeport" {
  metadata {
    name      = "mssql-nodeport-service"
    namespace = local.default_namespace
  }

  spec {
    type = "NodePort"

    selector = {
      "app" = "mssql"
    }

    port {
      name        = "default"
      protocol    = "TCP"
      port        = 1433
      target_port = 1433
    }
  }
}

resource "kubernetes_stateful_set" "mssql" {
  metadata {
    name      = "mssql-ss"
    namespace = local.default_namespace
  }

  spec {
    service_name = "mssql-clusterip-service"

    selector {
      match_labels = {
        "app" = "mssql"
      }
    }

    template {
      metadata {
        labels = {
          "app" = "mssql"
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
                name = "mssql-secret"
                key  = "sa-password"
              }
            }
          }

          volume_mount {
            name       = "mssqldb"
            mount_path = "/var/opt/mssql/data"
          }
        }

        volume {
          name = "mssqldb"

          persistent_volume_claim {
            claim_name = "mssql-pvc"
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "mssql_clusterip" {
  metadata {
    name      = "mssql-clusterip-service"
    namespace = local.default_namespace
  }

  spec {
    type = "ClusterIP"

    selector = {
      "app" = "mssql"
    }

    port {
      name        = "default"
      protocol    = "TCP"
      port        = 1433
      target_port = 1433
    }
  }
}
