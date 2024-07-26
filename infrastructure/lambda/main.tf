resource "aws_iam_role" "iam_for_lambda" {
  name               = local.function_name
  
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "lambda.amazonaws.com"
        }
      }
    ]
  })

  inline_policy {
    name = "dynamodb_access"
    policy = jsonencode({
      Version="2012-10-17"
      Statement=[{
        Action = [
          "dynamodb:DescribeTable",
          "dynamodb:PutItem",
          "dynamodb:Scan",
          "dynamodb:Query",
        ]
        Resource = [
          data.aws_dynamodb_table.statements.arn
        ]
        Effect="Allow"
      }]
    })
  }

  inline_policy {
    name = "s3_access"
    policy = jsonencode({
      Version="2012-10-17"
      Statement=[{
        Action = [
          "s3:*",
        ]
        Resource = [
          "${data.aws_s3_bucket.selected.arn}/*"
        ]
        Effect="Allow"
      }]
    })
  }
  
  inline_policy {
    name = "cloudwatch"
    policy = jsonencode({
      Version="2012-10-17"
      Statement=[{
        Action = [
          "logs:*",
        ]
        Resource = [
          "arn:aws:logs:*:${data.aws_caller_identity.current.id}:*",
          "arn:aws:logs:*:${data.aws_caller_identity.current.id}:log-group:/aws/lambda/*"
        ]
        Effect="Allow"
      }]
    })
  }
}

resource "aws_lambda_function" "tesco_handler" {
  s3_bucket = data.aws_s3_bucket.selected.id
  s3_key = "lambda-packages/${local.function_name}/package.zip"
  s3_object_version = "${var.s3_object_version}"
  function_name = local.function_name
  role          = aws_iam_role.iam_for_lambda.arn
  handler       = "TescoStatementProcessorLambda::TescoStatementProcessorLambda.Function::FunctionHandlerAsync"
  timeout = 60
  #source_code_hash = data.archive_file.lambda.output_base64sha256

  runtime = "dotnet8"

  environment {
    variables = {
      foo = "bar"
    }
  }
}