import { register } from 'node:module';

register(new URL('./loader.js', import.meta.url).href, import.meta.url);
