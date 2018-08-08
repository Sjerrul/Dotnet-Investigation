# Sjerrul.DotNetInvestigation
A collection of projects that investigate usage of various stuff in .NET, like specific Nuget Packages, libraries, etc



## Sjerrul.DotNetInvestigation.Transactions

A look into the System.TransactionScope class, to see how hard it is to make custom transactional classes. 

This originated from a problem where an application I build required both actions on a database and an AWS S3 bucket, and needed to be rolled back to a known state when either of those actions failed