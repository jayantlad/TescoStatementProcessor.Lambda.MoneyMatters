data "aws_s3_bucket" "selected" {
  bucket = "moneymatters"
}

data "aws_dynamodb_table" "tableName" {
  name = "Statements"
}