.thumb
push	{lr}
ldr	r3,[r1]
@check if the pointer is in the list
ldr	r0,poin
loop:
ldr	r2,[r0]
add	r0,#4
cmp	r2,#0
beq	vanilla
cmp	r2,r3
bne	loop

@if in the list, use the new method
ldrb	r0,[r3,#2]
ldrb	r1,[r3,#3]
mov	r2,#0	@ldrb	r2,[r3,#4]
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
pop	{pc}

@if not in the list, vanilla
vanilla:
ldr	r1,[r1]
ldrh	r0,[r1,#2]
ldrh	r1,[r1,#4]
ldr	r3,=#0x807C4C4
mov	lr,r3
.short	0xF800
pop	{pc}

.align
.ltorg
poin:
