.equ shake2, shake1+4
.thumb
push	{r4-r6,lr}
ldr	r4,=#0x300100C
ldrb	r4,[r4]
mov	r5,#1
and	r4,r5
cmp	r4,#0
beq	first
b	second

first:
ldr	r4,shake1
b	store

second:
ldr	r4,shake2
b	store

store:
ldr	r5,=#0x3000FB4
strh	r4,[r5]

ldr	r4,=#0x3000FD0
ldrh	r0,[r4,#8]
mov	r5,#0x1C
and	r5,r0
ldr	r3,=#0x804FD9B
bx	r3

.align
.ltorg
shake1:
