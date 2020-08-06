# CloudFormation Custom Resource Security

We have a number of different CloudFormation custom resources deployed to our Shared account. Many of these have cross-account functionality: for example, if you want to deploy an SSL Certificate in the Dev account, but the DNS Hosted Zone lives in the Shared account, the custom resource will get invoked by CloudFormation in the dev account. By default, the Dev account does not have permission to invoke custom resources that live in Shared. This repository sets up those permissions.

## Prerequisites

- Deploy [cfn](https://github.com/cythral/cfn)

## Usage

For each custom resource that needs to be invoked from either Dev or Prod, add an item to resources.yml:

```yml
- Name: ResourceName
  Export: NameOfExportedLambdaArn
  Grantees:
    - cfn-metadata:DevAgentRoleArn # Dev
    - cfn-metadata:ProdAgentRoleArn # Prod
```

- The value for the `Export` property should be the name of a CloudFormation export whose value is the ARN of a Custom Resource Lambda ARN.

Here's what the above example gets transformed to in the resulting CloudFormation template:

```yml
ResourceNamePermission0:
  Type: AWS::Lambda::Permission
  Properties:
    FunctionName: !ImportValue NameOfExportedLambdaArn
    Principal: !ImportValue cfn-metadata:DevAgentRoleArn
    Action: lambda:InvokeFunction

ResourceNamePermisssion1:
  Type: AWS::Lambda::Permission
  Properties:
    FunctionName: !ImportValue NameOfExportedLambdaArn
    Principal: !ImportValue cfn-metadata:ProdAgentRoleArn
    Action: lambda:InvokeFunction
```
