# Cloud.Storage
A library for abstracting away low-level details of the various different cloud storage providers such that you can program to one interface and interchangeably swap out providers behind the scenes without any code changes.  The other primary goal of this project is to implement the best, most performant practices for all the various providers without the user of the library ever needing to worry or know about the nuance details that are very rarely followed or known about.

This project will inevitably be deployed as a public nuget package.

Note: the following is what is currently planned as I am currently the only contributor to the project.  I would love to have other contributors, especially in the area of additional data providers.  If/when that happens, the roadmap is likely to change.  My only requirement (currently) is that, once a major release is made, the interface related to that release should not have any breaking changes.  Additions to the interfaces are fully welcome, but changing existing interfaces/method definitions would require significantly solid logical reasoning. 

# Project Roadmap

Version 1 - Blob, Table and Queue storage working for both AWS and Azure.  Currently focusing entirely on async/await methodology with plans to implement synchronous methods.

Version 2 - Document Storage working for Azure's DocumentDB, AWS's Simple Storage and MongoDB

Version 3 - Caching with support for at least Redis - others are TBD

Version 4 - Multiple datacenter redundancy for items in version 1.  I'm aiming to allow this to work with a mixture of cloud service providers (e.g. primary storage could be in Azure and secondary storage in AWS or vice versa).
The scenarios I'm considering are:

A) Failover - when primary storage is not available, failover to secondary storage.

B) Load balancing - using both primary and secondary storage concurrently to ensure the best performance

C) Geo-based load balancing - based on the location of the client, the closest datacenter will be used.  Ideally this will optionally include both failover and regular load balancing within the same region.

It is unlikely that all scenarios will be available upon each release.  (A) will likely be 4.1, (B) will likely be 4.2, (C) will likely be split across multiple minor revisions (probably 4.3, 4.4 and 4.5)



Version 5+N - Begin implementing additional data providers beyond AWS/Azure.  Exactly which and in what order is TBD.

# Versioning Strategy

{Major}.{Minor}.{Revision}

Major will correspond to each version listed in the roadmap.  I am going to make every possible attempt to never have any breaking changes in the interface of each major version.  This means, e.g. once version 1 is complete and I move to version 2, version 1's interface should not change and so on going forward.

Minor will correspond to a complete working implementation of each backing data provider for a given major version

Revision will correspond to individual feature implementations of a given backing data provider