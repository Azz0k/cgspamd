import {createRootRoute, createRoute, createRouter} from "@tanstack/react-router";
import {FilterRulesContent} from "@/components/filter-rules-content.tsx";
import {UsersContent} from "@/content/users/users-content.tsx";

const rootRoute = createRootRoute();
const indexRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: ()=><FilterRulesContent />,
});
const usersRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/users',
  component: ()=><UsersContent />,
});
const routeTree = rootRoute.addChildren(
  [
    indexRoute,
    usersRoute,
  ]);
export const router = createRouter({routeTree});