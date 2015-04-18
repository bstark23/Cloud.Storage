# Cloud.Storage
A library for abstracting away low-level details of the various different cloud storage providers such that you can program to one interface and interchangeably swap out providers behind the scenes without any code changes.

This project will inevitably be deployed as a public nuget package.

# Project Roadmap

Version 1 - Blob, Table and Queue storage working for both AWS and Azure.  Currently focusing entirely on async/await methodology with plans to implement synchronous methods.

Version 2 - Document Storage working for Azure's DocumentDB, AWS's Simple Storage and MongoDB

Version 3 - Caching with support for at least Redis - others are TBD

# Versioning Strategy

{Major}.{Minor}.{Revision}

Major will correspond to each version listed in the roadmap.  I am going to make every possible attempt to never have any breaking changes in the interface of each major version.  This means, e.g. once version 1 is complete and I move to version 2, version 1's interface should not change and so on going forward.

Minor will correspond to a complete working implementation of each backing data provider for a given major version

Revision will correspond to individual feature implementations of a given backing data provider