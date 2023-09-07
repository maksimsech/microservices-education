variable "namespace" {
  type = string
}

variable "name" {
  type = string
}

variable "image" {
  type = string
}

variable "config_map_key_ref_env" {
  type = list(object({
    env_name        = string
    config_map_name = string
    config_map_key  = string
  }))

  default = []
}

variable "secret_key_ref_env" {
  type = list(object({
    env_name    = string
    secret_name = string
    secret_key  = string
  }))

  default = []
}


variable "value_env" {
  type = list(object({
    name  = string
    value = string
  }))

  default = []
}

variable "config_data" {
  type = map(string)

  default = {}
}

variable "clusterip_ports" {
  type = list(object({
    name        = string
    protocol    = string
    port        = number
    target_port = number
  }))

  default = []
}
