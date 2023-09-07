output "configmap_name" {
  value = kubernetes_config_map.this.metadata[0].name
}

output "service_name" {
  value = kubernetes_service.clusterip.metadata[0].name
}
