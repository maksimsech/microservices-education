resource "kubernetes_config_map" "this" {
  metadata {
    name      = "${local.name}-configmap"
    namespace = var.namespace
  }

  data = var.config_data
}
