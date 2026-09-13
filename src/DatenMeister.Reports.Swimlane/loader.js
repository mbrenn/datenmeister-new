import { pathToFileURL } from 'node:url';
import path from 'node:path';

export async function resolve(specifier, context, nextResolve) {
  if (specifier.startsWith('/js/datenmeister/') || specifier.includes('/js/datenmeister/') || specifier.includes('\\js\\datenmeister\\')) {
    let rel;
    if (specifier.includes('/js/datenmeister/')) {
      rel = specifier.substring(specifier.indexOf('/js/datenmeister/') + '/js/datenmeister/'.length);
    } else {
      rel = specifier.substring(specifier.indexOf('\\js\\datenmeister\\') + '\\js\\datenmeister\\'.length);
    }
    const target = path.resolve(import.meta.dirname, '../Web/DatenMeister.WebServer/Assets/js/datenmeister', rel);
    return {
      format: 'module',
      shortCircuit: true,
      url: pathToFileURL(target).href
    };
  }

  return nextResolve(specifier, context);
}
