# TransactionScope
[Source here](https://www.codeguru.com/csharp/.net/net_data/sortinganditerating/article.php/c10993/SystemTransactions-Implement-Your-Own-Resource-Manager.htm)

## The two-phase transaction process

1. An RM enlists in the transaction.
2. The RM does its part, and signals the end of Phase 1 to the TM. This phase can also be referred to as the prepare phase.
3. The TM gives the green signal to all RMs after they all have executed the prepare phase successfully.
4. The RMs get the green signal and actually commit their work. In the event of a red signal, they roll back their work. This is the second phase, or the commit phase.
5. In either case, the transaction coordinator coordinates with all the RMs to ensure that they all either succeed and do the requested work, or they all roll back their work together.

## Glossary

- Durable resource: refers to a resource that would require failure recovery. A good example would be transactional file copy. If you had to implement an RM that encapsulated the process of copying a file into a transaction, the file could actually be copied in Phase 1. In the event that the RM goes down, its recovery contract ensures that in the event of a rollback, the TM has enough information to restore the original state. A rollback in transactional file copy could be to delete the file or replace it with the pre-existing file.
- Volatile resource: is one that doesn't require recovery. A good example is in-memory data structures.