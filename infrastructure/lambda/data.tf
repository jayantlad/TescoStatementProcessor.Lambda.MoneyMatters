data "aws_s3_bucket" "selected" {
  bucket = "moneymatters"
}

data "aws_dynamodb_table" "statements" {
  name = "Statements"
}

data aws_caller_identity current {}
data aws_region current {}