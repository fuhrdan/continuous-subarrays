//*****************************************************************************
//** 2762. Continuous Subarrays                                 leetcode     **
//*****************************************************************************

// A structure for a sorted multiset node
typedef struct Node {
    int value;
    int count;
    struct Node* left;
    struct Node* right;
} Node;

// Helper function to create a new node
Node* createNode(int value) {
    Node* node = (Node*)malloc(sizeof(Node));
    node->value = value;
    node->count = 1;
    node->left = NULL;
    node->right = NULL;
    return node;
}

// Insert a value into the multiset
Node* insert(Node* root, int value) {
    if (!root) {
        return createNode(value);
    }
    if (value < root->value) {
        root->left = insert(root->left, value);
    } else if (value > root->value) {
        root->right = insert(root->right, value);
    } else {
        root->count++;
    }
    return root;
}

// Remove a value from the multiset
Node* erase(Node* root, int value) {
    if (!root) {
        return NULL;
    }
    if (value < root->value) {
        root->left = erase(root->left, value);
    } else if (value > root->value) {
        root->right = erase(root->right, value);
    } else {
        if (root->count > 1) {
            root->count--;
        } else {
            if (!root->left) {
                Node* rightChild = root->right;
                free(root);
                return rightChild;
            } else if (!root->right) {
                Node* leftChild = root->left;
                free(root);
                return leftChild;
            } else {
                Node* successor = root->right;
                while (successor->left) {
                    successor = successor->left;
                }
                root->value = successor->value;
                root->count = successor->count;
                successor->count = 1;
                root->right = erase(root->right, successor->value);
            }
        }
    }
    return root;
}

// Get the minimum value in the multiset
int getMin(Node* root) {
    while (root->left) {
        root = root->left;
    }
    return root->value;
}

// Get the maximum value in the multiset
int getMax(Node* root) {
    while (root->right) {
        root = root->right;
    }
    return root->value;
}

// Free the multiset
void freeMultiset(Node* root) {
    if (!root) {
        return;
    }
    freeMultiset(root->left);
    freeMultiset(root->right);
    free(root);
}

long long continuousSubarrays(int* nums, int numsSize) {
    long long ans = 0;
    int i = 0;
    Node* s = NULL; // Multiset root

    for (int j = 0; j < numsSize; ++j) {
        s = insert(s, nums[j]);
        while (getMax(s) - getMin(s) > 2) {
            s = erase(s, nums[i++]);
        }
        ans += j - i + 1;
    }

    freeMultiset(s);
    return ans;
}