resource "kubernetes_ingress_v1" "this" {
  metadata {
    name      = "ingress-service"
    namespace = local.default_namespace
    annotations = {
      "kubernetes.io/ingress.class"           = "nginx"
      "nginx.ingress.kubernetes.io/use-regex" = "true"
    }
    labels = {
      "name" = "ingress-service"
    }
  }

  spec {
    rule {
      host = "platforms.mng-microservice-education.net"
      http {
        path {
          path      = "/"
          path_type = "Prefix"
          backend {
            service {
              name = "platform-clusterip-service"
              port {
                number = 80
              }
            }
          }
        }
      }
    }

    rule {
      host = "commands.mng-microservice-education.net"
      http {
        path {
          path      = "/"
          path_type = "Prefix"
          backend {
            service {
              name = "command-clusterip-service"
              port {
                number = 80
              }
            }
          }
        }
      }
    }
  }
}
