# YAML File Format Documentation

Presets and Mystery weights can be stored using YAML files. Saving custom presets can be handled entirely through the randomizer UI, as well as loading them. However, if you want to create custom Mystery weightsets, you will have to manually edit YAML files.

The best starting point for creating custom Mystery weights is either saving a Mystery template YAML using the respective menu button in the randomizer UI, or modifying a copy of any existing weights file. For most use cases, it will be enough to simply change some numbers.

This document only explains the data the randomizer is expecting to read from a given YAML file, this is not a general YAML specification.

We highly recommend looking through one of the YAML files included in the randomizer alongside reading this for much easier understanding.

## The root node

The root note of the YAML should consist of the following scalars and mappings:

- `name`: A string denoting the name for the preset or weightset. We recommend to have it similar to the file name.
- `description`: A string describing what the preset or weightset roughly consists of, or when it should be used, for example "Anything is possible besides hard logic tricks.". Currently unused in the randomizer, but might be used in a future version or by other tools using the randomizer core.
- `settings` (optional): A mapping from randomizer options to some kind of data. This data can either be the value to set that option to, or a way of randomly determining one out of multiple options. More information below.
- `subweights` (optional): A mapping of subweight groups named by their keys. Each subweight group is a mapping of subweightsets named by their keys. Each subweightset is a node structured similarly to the root node. More information below.

## The `settings` node

This is a mapping where the keys are the define names of options defined in the randomizer logic file. The corresponding value can either be an option value that this option should be set to, or a choice node used for a random weighted choice from a set of option values.

The format for valid option values depends on the option type:

- Flags: A string taking the value `true` or `false` for enabling or disabling the flag respectively. The strings `on`, `off`, `1`, `0` and other capitalizations are also accepted.
- Dropdowns: A string consisting of the name for the define of this dropdown entry as defined in the logic file.
- Numberboxes: A string consisting of 1 to 3 integers separated by spaces. The allowed minimum and maximum values for this option need to be respected in all 3 cases.
    - 1 integer: The value to set this option to.
    - 2 integers: A range of possible values to set this option to. The randomizer will pick a random number greater than or equal to the first number and less than or equal to the second number, uniformly distributed.
    - 3 integers: Like with 2 integers, except the third number is used as a step value. This means only values that are an integer multiple of the step value away from the first number can be picked.
- Colors: A string consisting of either a space-separated list of 3 integers from 0 to 255 denoting the red, green and blue color values respectively (like `16 255 8`), or a HTML color string (like `#10FF08`), or one of the following keywords, ignoring case:
    - `vanilla`: The color value is not overwritten, or more generally: The define for this option is not set in the shuffling process.
    - `default`: The default color is used.
    - `random`: On each seed generation, a random color value will be picked.

A choice node is a mapping from option values to non-negative integers denoting their weights. The randomizer will pick a random weighted choice from the supplied selection of option values. For example, a value with a weight of 20 will be 4 times as likely to be picked as a value with weight 5 in the same choice node.

Options that are not part of the used logic file will be completely ignored.

Trying to set an option to an illegal value will cause an error.

While processing a YAML file, the randomizer makes no distinction between logic settings and cosmetic settings, however the user may choose to only use one of these 2 option types from the YAML processing result.

## The `subweights` node

Subweights can be used to bundle together combinations of options, for example to always or often use the same random setting for Red, Blue and Green Fusions, or to have different requirement distributions based on the chosen DHC setting.

This node is a mapping of subweight groups named by their keys. Each subweight group is a mapping of subweightsets named by their keys.

Each subweightset is a node structured similarly to the root node. In addition to the `settings` and `subweights` mappings which work exactly like in the root node, there needs to be a `chance` string containing a non-negative integer. This chance value is used for picking a random weighted choice of a subweightset within a subweight group.

For the chosen subweightset within each subweight group, the `settings` and `subweights` are processed like before.

Below is an example of using a subweight group called `coupled_fusions` with the 5 subweightsets `removed`, `vanilla`, `combined`, `open` and `decoupled`. The latter (which has a 1/3 chance) applies no further changes to any option values. The other 4 (with a 1/6 chance each) set the Red, Blue and Green Fusion settings to the same fixed value. When this YAML is loaded and the `settings` node has been processed, one out of these 5 subweights is randomly selected and the mentioned options are overwritten.
```
subweights:
  coupled_fusions:
    removed:
      chance: 1
      settings:
        RED_FUSION_SETTING: NO_RED_FUSIONS
        BLUE_FUSION_SETTING: NO_BLUE_FUSIONS
        GREEN_FUSION_SETTING: NO_GREEN_FUSIONS
    vanilla:
      chance: 1
      settings:
        RED_FUSION_SETTING: VANILLA_RED_FUSIONS
        BLUE_FUSION_SETTING: VANILLA_BLUE_FUSIONS
        GREEN_FUSION_SETTING: VANILLA_GREEN_FUSIONS
    combined:
      chance: 1
      settings:
        RED_FUSION_SETTING: COMBINED_RED_FUSIONS
        BLUE_FUSION_SETTING: COMBINED_BLUE_FUSIONS
        GREEN_FUSION_SETTING: COMBINED_GREEN_FUSIONS
    open:
      chance: 1
      settings:
        RED_FUSION_SETTING: OPEN_RED_FUSIONS
        BLUE_FUSION_SETTING: OPEN_BLUE_FUSIONS
        GREEN_FUSION_SETTING: OPEN_GREEN_FUSIONS
    decoupled:
      chance: 2
      settings: {}
```

## Priority

Before processing starts, each option will be assigned its default value as specified in the logic file.

In each weightset, the `settings` node is processed before the `subweights` node. Whenever an option is set to a value, the previous value will be replaced.

The order in which multiple subweight groups within the same node are processed is undefined.
