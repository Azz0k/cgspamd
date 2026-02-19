
import {filterRuleTypes} from "@/content/filter-rules/components/filter-rule-types.ts";
import type {FilterRule} from "@/content/filter-rules/filter-rules-columns.tsx";
import type {Table} from "@tanstack/react-table";
import {useState} from "react";
import {
  DropdownMenu,
  DropdownMenuContent, DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuTrigger
} from "@/components/ui/dropdown-menu.tsx";
import {Filter, FilterX, LucideCheck} from "lucide-react";
import {Button} from "@/components/ui/button.tsx";

type TypeHeaderProps = {
  table:  Table<FilterRule>;
}

export const TypeHeader = ({table}:TypeHeaderProps)=>{
  const [selected, SetSelected] = useState<string | null>(null);
  const handleSelect = (element:string)=>{
    if (element === selected){
      SetSelected(null);
      table.getColumn("type")?.setFilterValue(undefined);
    }
    else{
      SetSelected(element);
      let typeNumber:number = 0;
      filterRuleTypes.forEach((rule, index)=>{
        if (rule===element){
          typeNumber = index;
        }
      });
      table.getColumn("type")?.setFilterValue(typeNumber);
    }
  }
  const values = filterRuleTypes.map(element=>
    <DropdownMenuItem
      key={element}
      onSelect={()=>handleSelect(element)}
    >
      {element===selected && <LucideCheck/>}
      {element}
    </DropdownMenuItem>
  )
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <div className="flex justify-between w-full">
          <span>Тип</span>
          <Button size="xs" variant="ghost">
            <span className="sr-only">Open menu</span>
            {selected===null?<Filter/>:<FilterX/>}
          </Button>
        </div>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="start">
        <DropdownMenuGroup>
          {values}
        </DropdownMenuGroup>
      </DropdownMenuContent>
    </DropdownMenu>
  );
}