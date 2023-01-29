# Contributing to Minish Cap Randomizer

Thank you for your interest in contributing to the project! Check out the info below to learn how best you can help this
program evolve.

First off, all of our development chat can be found over on our community [Discord](https://minishmaker.com/discord).
Please join it if you wish to contribute!

## Bug Reports and Feature Requests

### Bug Reports

To create a bug report, please open an [issue](https://github.com/minishmaker/randomizer/issues) on our GitHub. When
reporting, please make sure you give all the information possible so that we can reproduce the error. A good format to
follow would be:

- Short description of the bug
- Clear steps needed to reproduce the bug
- What you expected to happen when performing the steps
- What actually happened
- System settings, project version etc.

Once you've posted, drop us a message at the Discord in `#minishmaker-dev` so that we're aware and can take a look.

### Feature Requests

The project is still in it's infancy and we'd love to know what features you'd be interested in! To make a feature
request, please open an [issue](https://github.com/minishmaker/randomizer/issues) on our GitHub, and just like the bug
reports, let us know in the Discord. A good format for feature requests would be:

- Description of the feature
- Why you think it would be a good addition to the program
- How you would like to see it implemented (what menus, buttons, interfaces etc.)

## Development and Contribution

### Downloading and building from source

The project is built on Windows. Once the general feature set has been implemented we'll look at cross platform support.
Therefore, the following programs are required to build the project:

- Visual Studio 2019
- Git
- devkitPro (optional for assembly programming)

Fork and clone the project repository in the usual way. Once you've cloned, you'll need to
run `git submodule update --init --recursive` to fetch the submodules the project depends on, namely ColorzCore for the
build system. Open the project solution in Visual Studio and you'll be good to go!

### Implementing Feature Requests or Bug Fixes

If you see a FR/bug on the [issues](https://github.com/minishmaker/randomizer/issues) list that you'd like to work on,
please comment on the issue to notify us. This helps us keep track of what features are being worked on, and will help
to prevent multiple implementations. Make sure that you branch from `staging` when developing. Come join us in
the `#minishmaker-dev` chat on Discord to discuss it!

### Features not listed on GitHub

If you're planning on implementing a new feature that doesn't have a ticket, we'd appreciate it if you'd let us know
first by opening up a new [issue](https://github.com/minishmaker/randomizer/issues) as above, and messaging us on
Discord. This is purely to prevent unnecessary work in the (unlikely) event that we don't think the feature is right for
the project at the time.

### Pull Requests

Once you're happy with your additions, please open a [Pull Request](https://github.com/minishmaker/randomizer/pulls)
that targets the `staging` branch.
We'll check it to make sure we'll happy with it, and hopefully get it merged quickly! Once your PR has been merged,
you'll get the Contributor role on the Discord and your choice of name added in a Contributor's section on the program's
About window!
