module "lambda"{
    source = "./lambda"
    s3_object_version = var.s3_object_version
}