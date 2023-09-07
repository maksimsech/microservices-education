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
      name        = "default"
      protocol    = "TCP"
      port        = 1433
      target_port = var.port
    }
  }
}
