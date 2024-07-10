data "aws_iam_policy_document" "assume_role" {
  statement {
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }

    actions = ["sts:AssumeRole"]
  }
}

resource "aws_iam_role" "iam_for_lambda" {
  name               = local.function_name
  assume_role_policy = data.aws_iam_policy_document.assume_role.json
}

resource "aws_lambda_function" "tesco_handler" {
  # If the file is not in the current working directory you will need to include a
  # path.module in the filename.
  s3_bucket = data.aws_s3_bucket.selected.id
  s3_key = local.function_name
  function_name = local.function_name
  role          = aws_iam_role.iam_for_lambda.arn
  handler       = "TescoStatementProcessorLambda::TescoStatementProcessorLambda.Function::FunctionHandler"

  #source_code_hash = data.archive_file.lambda.output_base64sha256

  runtime = "dotnet8"

  environment {
    variables = {
      foo = "bar"
    }
  }
}