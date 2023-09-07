resource "kubernetes_config_map" "this" {
  metadata {
    name      = "${var.name}-config"
    namespace = var.namespace
  }

  data = {
    "${local.config_host_key}" = "${local.service_name}"
    "${local.config_port_key}" = "${var.messaging_port}"
  }
}
