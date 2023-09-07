locals {
  service_name        = "${var.name}-clusterip"
  secret_password_key = "password"

  config_db_url_key = "database-url"
}
