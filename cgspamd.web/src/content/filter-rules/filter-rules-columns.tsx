import '@tanstack/react-table';
import {type ColumnDef } from "@tanstack/react-table";
import {FilterRuleActions} from "@/content/filter-rules/components/filter-rule-actions.tsx";
import {filterRuleTypes} from "@/content/filter-rules/components/filter-rule-types.ts";
import { TypeHeader} from "@/content/filter-rules/components/TypeHeader.tsx";

export type FilterRule = {
  id: number;
  value:string;
  comment: string;
  type: number;
  createdAt: string;
  updatedAt?: string;
  createdByUserName:string;
  updatedByUserName?:string;
};

export const filterRuleColumns:ColumnDef<FilterRule>[] = [
  {
    accessorKey: "value",
    header: "Значение",
    meta: {headerClassName: "w-2/10", },
  },
  {
    accessorKey: "comment",
    header: "Комментарий",
    meta: {headerClassName: "w-2/10", },
  },
  {
    header: ({table})=><TypeHeader table={table}/>,
    id: "type",
    accessorKey: "type",
    meta: {
      headerClassName: "w-1/10",
    },
    cell: ({ row }) => {
      const filterRule = row.original;
      return (
        <span>
          {filterRuleTypes[filterRule.type]}
        </span>
      )
    },
    enableColumnFilter: true,
    filterFn: (row, id, value) =>{
      if (id==="type"){
        return row.original.type === value
      }
      return false
    }
  },
  {
    accessorKey: "createdAt",
    header: "Когда создано",
    meta: {headerClassName: "w-1/10", },
  },
  {
    accessorKey: "createdByUserName",
    header: "Кем создано",
    meta: {headerClassName: "w-1/10", },
  },
  {
    accessorKey: "updatedAt",
    header: "Когда изменено",
    meta: {headerClassName: "w-1/10", },
  },
  {
    accessorKey: "updatedByUserName",
    header: "Кем изменено",
    meta: {headerClassName: "w-1/10", },
  },

  {
    id: "actions",
    header: "Действия",
    cell: ({ row }) => {
      const filterRule = row.original;
      return (
        <FilterRuleActions filterRule={filterRule}/>
      )
    },
    meta: {headerClassName: "w-1/10", }
  },
];

