resource "kubernetes_config_map" "this" {
  metadata {
    name      = "${var.name}-config"
    namespace = var.namespace
  }

  data = {
    "${local.config_db_url_key}" = "${local.service_name}, ${var.port}"
  }
}
