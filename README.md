## Project Setup

- This repository uses Git LFS. Install it [here](https://git-lfs.com/) and afterwards run git lfs pull to pull all assets
- Install Godot v4.5.1 with C# Support. [Link for Linux](https://downloads.godotengine.org/?version=4.5.1&flavor=stable&slug=mono_linux_x86_64.zip&platform=linux.64)
- Open the project in godot and done

## Editor Setup

- VSCode:
  - Install the C# extension. That should suffice
- Neovim:
  - If version >= 0.11.0:
      ```lua
      vim.lsp.enable("csharp_ls")
      ```
  - If version below, setup LSP server `csharp_ls` like other LSP servers in your config
  - Regardless of version, install the lsp server as documented [here](https://github.com/razzmatazz/csharp-language-server?tab=readme-ov-file#quick-start)

## Roadmap

- [ ] Milestone 1
    - Player Movement
    - A small island
    - Grow plants
    - Buy/Sell plants
    - Buy new grow plots and be able to place them somewhere on your island

## Assets

- 3D Models: Please use `glb` format if possible
