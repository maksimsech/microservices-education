variable "namespace" {
  type = string
}

variable "name" {
  type = string
}

variable "management_port" {
  type = number

  validation {
    condition     = var.management_port > 0 && var.management_port < 65536
    error_message = "Enter valid port value"
  }
}

variable "messaging_port" {
  type = number

  validation {
    condition     = var.messaging_port > 0 && var.messaging_port < 65536
    error_message = "Enter valid port value"
  }
}

variable "share_via_nodeport" {
  type = bool

  default = false
}
