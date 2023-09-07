resource "kubernetes_service" "clusterip" {
  metadata {
    name      = local.service_name
    namespace = var.namespace
  }

  spec {
    type = "ClusterIP"

    selector = {
      "app" = var.name
    }

    port {
      name        = "management"
      protocol    = "TCP"
      port        = 15672
      target_port = var.management_port
    }

    port {
      name        = "messaging"
      protocol    = "TCP"
      port        = 5672
      target_port = var.messaging_port
    }
  }
}
