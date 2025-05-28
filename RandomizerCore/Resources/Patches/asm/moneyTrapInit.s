.thumb
ldrb	r0,[r4,#0xA]
cmp	r0,#0
bne	noDeletion
ldrb	r0,[r4,#0xB]
cmp	r0,#0
bne	noDeletion

mov	r0,#0x63
@ CheckLocalFlag
ldr	r3,=#0x807C5F4
mov	lr,r3
.short	0xF800
cmp	r0,#0
beq	noDeletion

ldr	r3,=#0x8085CA3
bx	r3

noDeletion:
ldr	r3,=#0x8085CA7
bx	r3
