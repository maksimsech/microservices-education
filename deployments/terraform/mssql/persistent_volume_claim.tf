resource "kubernetes_persistent_volume_claim" "this" {
  metadata {
    name      = "${var.name}-pvc"
    namespace = var.namespace
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
