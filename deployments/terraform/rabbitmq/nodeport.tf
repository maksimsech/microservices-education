resource "kubernetes_service" "nodeport" {
  count = var.share_via_nodeport ? 1 : 0

  metadata {
    name      = "${var.name}-nodeport"
    namespace = var.namespace
  }

  spec {
    type = "NodePort"

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
