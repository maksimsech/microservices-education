resource "kubernetes_secret" "this" {
  metadata {
    name      = "${var.name}-secret"
    namespace = var.namespace
  }

  type = "Opaque"
  data = {
    "${local.secret_password_key}" = var.password
  }
}
