resource "kubernetes_service" "clusterip" {
  metadata {
    name      = "${local.name}-clusterip"
    namespace = var.namespace
  }

  spec {
    type = "ClusterIP"

    selector = {
      "app" = local.name
    }

    dynamic "port" {
      for_each = var.clusterip_ports
      content {
        name        = port.value.name
        protocol    = port.value.protocol
        port        = port.value.port
        target_port = port.value.target_port
      }
    }
  }
}
