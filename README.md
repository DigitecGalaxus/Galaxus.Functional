# Galaxus.Functional

## Introduction

`Galaxus.Functional` is a package that aims to bring FP-style features to C#. Note though, that this is not true functional programming;
this package contains some useful abstractions often found in functional languages.

The style is greatly inspired by the way Rust does this. If you have worked with Rust's [std::option](https://doc.rust-lang.org/std/option/index.html)
or [std::result](https://doc.rust-lang.org/std/result/index.html), you will probably feel right at home.

## Abstractions

There currently are the following abstractions found inside `Galaxus.Functional`:

#### Option

A type used to explicitly mark the presence or absence of a value. This is an alternative to checking for `null` and is similar to
e.g. Haskell's `Maybe` or Java's `Optional`.

#### Unit

A type for something that does not exist, similar to `void` - except that it can be returned and stored as a value.

#### Either

A type that can be one of several variants, sort of like a discriminated union or an enum with a stored value for each enum member.

#### Result

A type that explicitly propagates success or error to the caller, similar to used explicitly annotated exceptions. It can be seen
as a specialization of an `Either` with the second variant being an error.

## Contribute

No matter how small, we value every contribution! If you wish to contribute,

1. Please create an issue first - this way, we can discuss the feature and flesh out the nitty-gritty details
2. Fork the repository, implement the feature and submit a pull request
3. Add yourself to the CONTRIBUTORS.txt file in that pull request
4. Once the maintainers have released a new version, your feature will be part of the NuGet package

And always remember the golden rule for every contribution:
[Be excellent to each other!](https://www.youtube.com/watch?v=rph_1DODXDU)
