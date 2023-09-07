output "configmap_name" {
  value = kubernetes_config_map.this.metadata[0].name
}

output "config_host_key" {
  value = local.config_host_key
}

output "config_port_key" {
  value = local.config_port_key
}
