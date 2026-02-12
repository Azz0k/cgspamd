"use client";

import { Moon, Sun } from "lucide-react";

import { Switch } from "@/components/ui/switch";
import {observer} from "mobx-react";
import {rootStore} from "@/store/root-store.ts";

export const ThemeSwitcher = observer(() => {

  return (
    <div className="flex items-center space-x-3">
      <Sun className="size-4" />
      <Switch
        checked={rootStore.themeSwitchValue}
        onCheckedChange={rootStore.handleCheckTheme}
        aria-label="Toggle theme"
      />
      <Moon className="size-4" />
    </div>
  );
});