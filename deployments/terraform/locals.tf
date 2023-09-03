locals {
  default_namespace = kubernetes_namespace.this.metadata[0].name
}