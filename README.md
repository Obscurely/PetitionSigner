# PetitionSigner

An app designed to spam sign petitions on https://campaniamea.declic.ro/petitions/
using proxies in order to avoid ip block and protect identity.
Also multi threaded in order to ensure spamming more efficient.

## About proxies

<<<<<<< HEAD
The proxies are gathered from https://proxyscrape.com/free-proxy-list they are http proxies used with a 3000ms max timeout set. These proxies are slow because they are public and a lot o people just use them like this so if you want high speed you would want a paid service that can give you a fast proxy on demand, if you would get that and a request would take 1 second and 32 threads do it all together so there is 32 signs per second meaning 115200 per hours that would be insane, but also very expensive.
=======
The proxies are gathered from https://proxyscrape.com/free-proxy-list they are http proxies used with a 3000ms max timeout set. These proxies are slow because they are public and a lot o people just use them like this so if you want high speed you would want a paid service that can give you a fast proxy on demand, if you would get that and a request would take 1 second and 32 threads do it all togheter so there is 32 signs per second meaning 115200 per hours that would be insane, but also very expensive.
>>>>>>> cb8f2660a66905261453e87392d3be6ff77fc967

## About multi threading

It uses multiple threads each trying random proxies from the lists they downloaded
and spam sign petitions together. By default the way is configured in Program.cs it uses 16 threads and that works with a processor with 8 threads (not cores), the number of threads has to be the double of the threads your cpu, if you use more the app will freeze after sending a few requests.

## About how it signs the petitions

It signs a petition with petition by picking random data from the given files, first_name.txt, last_names.txt and counties.txt. The app manipulates this data and creates a person object in order to use to make the request to sign this way: <div>

Creates a person object by choosing a random firstName, lastName, a county and a generated email, based on the information in the given lists.
An email would look like this firstName/lastName$SEPARATORlastName/firstName(opt->)$SEPARATORrandomNumber(in range 0-9999)@randomDomain where firstName and lastName
have each a 10% to be cut in half, a 50% chance for firstName to be first and 50% for lastName to be first, a 50% to include a number, $SEPARATOR
can be one of these { "", ".", "-", "-" } each having a 25% chance and a randomDomain from these { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "protonmail.com" }
each having 20% chance to be in the email.
The firstName and lastName have each a 20% chance of being written in lower case and a 33.33% (1 in 3) chance to have diacritics if they are written with.
The county is simply selected random from the list.
