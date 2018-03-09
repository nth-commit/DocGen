import { Template, InputValue, InputValueCollection } from '../models';

export const InputValueCollectionUtility = {

    fromTemplate: (template: Template): InputValueCollection => {
        const result: InputValueCollection = {};

        template.steps.forEach(s => {
          s.inputs.forEach(i => {
            result[i.id] = null;
          });
        });

        return result;
    },

    toEncoded: (collection: InputValueCollection) => {
        const result = btoa(JSON.stringify(collection));

        if (result.length > 2000) {
            // Possibly too long to put in a URL
            throw new Error('Value was too large to encode');
        }

        return result;
    },

    fromEncoded: (encoded: string) => {
        const result: InputValueCollection = JSON.parse(atob(encoded));

        Object.keys(result).forEach(k => {
            const type = typeof result[k];
            if (type !== 'string' && type !== 'boolean') {
                throw new Error('Encoded string was in an incorrect format');
            }
        });

        return result;
    },

    isEqual: (a: InputValueCollection, b: InputValueCollection) => {
        if (a === b) {
            return true;
        } else if (a === undefined || b === undefined) {
            return false;
        }

        const aKeys = Object.keys(a);
        const bKeys = Object.keys(b);

        if (aKeys.length !== bKeys.length) {
            return false;
        }

        return aKeys.every((aKey, i) => {
            const bKey = bKeys[i];
            if (!bKey) {
                return false;
            }

            return a[aKey] === b[bKey];
        });
    }
};
