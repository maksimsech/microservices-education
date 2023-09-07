variable "namespace" {
  type = string
}

variable "name" {
  type = string
}

variable "password" {
  type = string

  sensitive = true
}

variable "port" {
  type = number

  validation {
    condition     = var.port > 0 && var.port < 65536
    error_message = "Enter valid port value"
  }
}
