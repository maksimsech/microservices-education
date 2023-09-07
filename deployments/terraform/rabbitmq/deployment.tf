resource "kubernetes_deployment" "this" {
  metadata {
    name      = "${var.name}-deployment"
    namespace = var.namespace
  }

  spec {
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
          name  = var.name
          image = "rabbitmq:3.9.20-management"
          port {
            container_port = 15672
          }
          port {
            container_port = 5672
          }
        }
      }
    }
  }
}
