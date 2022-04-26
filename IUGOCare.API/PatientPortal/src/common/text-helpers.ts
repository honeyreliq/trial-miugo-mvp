const replacer = (_: string, ...args: any[]): string => {
  return args[0] + args[1].toUpperCase();
};

export const kebabToCamelCase = (s: string): string =>
  s.replace(/(\w+)-(\w)/g, replacer);
