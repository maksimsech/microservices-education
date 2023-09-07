output "configmap_name" {
  value = kubernetes_config_map.this.metadata[0].name
}

output "secret_name" {
  value = kubernetes_secret.this.metadata[0].name
}

output "config_db_url_key" {
  value = local.config_db_url_key
}

output "secret_password_key" {
  value = local.secret_password_key
}
