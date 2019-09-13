.thumb
push	{r4,r5,lr}
mov	r5,r0

ldr	r0,list
loop:
ldrb	r1,[r0]
cmp	r1,#0
beq	equip
cmp	r1,r5
beq	noEquip
add	r0,#1
b	loop

cmp	r5,#0x46
bhi	end
equip:
ldr	r3,=#0x8053F39
bx	r3
end:
ldr	r3,=#0x8053F41
bx	r3

noEquip:
ldr	r3,=#0x8053FA9
bx	r3
.align
.ltorg
list:
