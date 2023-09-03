resource "kubernetes_namespace" "this" {
  metadata {
    name = var.namespace_name

    labels = {
      name = var.namespace_name
    }
  }
}