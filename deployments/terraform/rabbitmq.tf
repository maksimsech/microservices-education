resource "kubernetes_deployment" "rabbitmq" {
  metadata {
    name      = "rabbitmq-deployment"
    namespace = local.default_namespace
  }

  spec {
    selector {
      match_labels = {
        "app" = "rabbitmq"
      }
    }

    template {
      metadata {
        labels = {
          "app" = "rabbitmq"
        }
      }

      spec {
        container {
          name  = "rabbitmq"
          image = "rabbitmq:3.9.20-management"
          port {
            container_port = 15672
          }
          port {
            container_port = 5672
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "rabbitmq_clusterip" {
  metadata {
    name      = "rabbitmq-clusterip-service"
    namespace = local.default_namespace
  }

  spec {
    type = "ClusterIP"

    selector = {
      "app" = "rabbitmq"
    }

    port {
      name        = "management"
      protocol    = "TCP"
      port        = 15672
      target_port = 15672
    }

    port {
      name        = "messaging"
      protocol    = "TCP"
      port        = 5672
      target_port = 5672
    }
  }
}

resource "kubernetes_config_map" "rabbitmq_configmap" {
  metadata {
    name      = "rabbitmq-configmap"
    namespace = local.default_namespace
  }

  data = {
    "host" = "rabbitmq-clusterip-service"
    "port" = "5672"
  }
}

resource "kubernetes_service" "rabbitmq_nodeport" {
  metadata {
    name      = "rabbitmq-nodeport-service"
    namespace = local.default_namespace
  }

  spec {
    type = "NodePort"

    selector = {
      "app" = "rabbitmq"
    }

    port {
      name        = "management"
      protocol    = "TCP"
      port        = 15672
      target_port = 15672
    }

    port {
      name        = "messaging"
      protocol    = "TCP"
      port        = 5672
      target_port = 5672
    }
  }
}
